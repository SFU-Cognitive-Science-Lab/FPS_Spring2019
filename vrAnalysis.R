#Uncomment the install packages line if you don't have either of these installed
#install.packages("lme4")
library(lme4)
#install.packages("dplyr")
library(dplyr)
#install.packages("ggplot2")
library(ggplot2)
#install.packages("car")
library(car)
#install.packages("lmerTest")
library(lmerTest)

#Set the directory to wherever you have the file installed
setwd("C:/Users/Justin 2/Desktop/School Docs/CSL/VR")
#This is the most current master table file on vault
dat <- read.csv("vrMasterTableNoET.csv", na.strings = "NaN")
View(dat)

#Create a lag variable for further models
dat$lag1 <- lag(dat$meanAccuracy, 1)
#This makes 1st bin NA (instead of carrying over from last participant)
dat$lag1[dat$bin == 1] <- NA

#Create a new dataset with just learners
learned <- subset(dat, subset = dat$learner == 1)



#-----Descriptive-----

#this gives basic descriptions of all variables
summary(dat)
#RT and FBduration still have negatives, so I won't use those in analyses

#RT subset (shows which participants have negative RT scores)
dat1 <- subset(dat, subset = dat$meanRT < 0)
dat1

#FBduration subset (shows which participants have negative FBduration scores)
dat2 <- subset(dat, subset = dat$meanFBDuration < 0)
dat2



#This just shows means for the listed variables for each group
dat %>%
  group_by(condition) %>%
  summarize(accuracy = mean(meanAccuracy, na.rm = T), optimization = mean(optimization, na.rm = T), cp = mean(cp, na.rm = T),
            fixDuration = mean(fixDuration, na.rm = T), fixCount = mean(fixCount, na.rm = T), learn_ratio = sum(learner)/n())

#run the same commands for the learned dataset
summary(learned)
#RT and FBduration also have negatives here

learned %>%
  group_by(condition) %>%
  summarize(accuracy = mean(meanAccuracy, na.rm = T), optimization = mean(optimization, na.rm = T), cp = mean(cp, na.rm = T),
            fixDuration = mean(fixDuration, na.rm = T), fixCount = mean(fixCount, na.rm = T))



#NOTE: all assumption checking and model fitting work is down below the analyses





#-----Chi Square-----

#The goodness of fit test looks for a difference in proportion of learners between the two groups
chi <- chisq.test(dat$learner, dat$condition)
chi
#These show the count tables used
chi$observed
chi$expected
#Test is significant and the tables show there is a higher proportion of learners in VR than 3D





#-----Mann-Whitney U Test-----

#Since normality assumption of T test was heavily violated, I used the non-parametric Mann-Whitney U Test (uses median instead of mean)
#tests if there's a difference in cp between the 2 groups (only considering those which met a cp) 
wilcox.test(learned$cp[learned$condition == "3D"],learned$cp[learned$condition == "VR"])
#There is no difference in criterion points between VR and 3D





#-----Modelling-----

#These controll for bin and subject effects and see which variables VR differs from 3D
#the 4 t tests were considered at a bonferroni corrected significance level (.05/4 = .0125)
#Accuracy
fit.accuracy <- lmer(meanAccuracy ~ poly(bin, 4) + condition + (1 | subject), data = learned)
summary(fit.accuracy)
#this predicts values of the model so that it can be graphed later
predict.accuracy <- data.frame(y.hat = predict(fit.accuracy, learned), bin = learned$bin)
#When controlling for bin and subject, condition does not have a significant effect (t = .137, p = .89120)

#Optimization
fit.optimization <- lmer(optimization ~ bin + condition + (1 | subject), data = learned)
summary(fit.optimization)
predict.optimization <- data.frame(y.hat = predict(fit.optimization, learned), bin = learned$bin)
#When controlling for bin and subject, condition does not have a significant effect (t = 1.528, p = .1320)

#Fixation Duration (multiple assumption violations, ANOVA might have high error rates)
  #create dataset to exclude outliers (and NAs) excluded trial 1 from subject 20414 and all trials from subject 30513 
learned1 <- subset(learned, subset = learned$fixDuration < 10000)
fit.fixDuration <- lmer(fixDuration ~ poly(bin, 3) + condition + (1 | subject), data = learned1)
summary(fit.fixDuration)
predict.fixDuration <- data.frame(y.hat = predict(fit.fixDuration, learned1), bin = learned1$bin)
#When controlling for bin and subject, condition does not have a significant effect (t = -.371, p = .712)

#Fixation Count
fit.fixCount <- lmer(fixCount ~ poly(bin, 3) + condition + (1 | subject), data = learned)
summary(fit.fixCount)
predict.fixCount <- data.frame(y.hat = predict(fit.fixCount, learned), bin = learned$bin)
#When controlling for bin and subject, condition has a significant effect (t = 2.636, p = .0108)





#-----Graphing-----

#this is a boxplot with smooth line for the dataset (blue) and smooth line for best model (red)
#Accuracy
ggplot(learned, aes(bin, meanAccuracy)) +
  geom_boxplot(mapping = aes(as.factor(bin), meanAccuracy, color = condition)) +
  geom_smooth(method = "loess") +
  geom_smooth(mapping = aes(bin, y.hat), data = predict.accuracy, method = "loess", color = 'red')

#Optimization
ggplot(learned, aes(bin, optimization)) +
  geom_boxplot(mapping = aes(as.factor(bin), optimization, color = condition)) +
  geom_smooth(method = "loess") +
  geom_smooth(mapping = aes(bin, y.hat), data = predict.optimization, method = "loess", color = 'red')

#Fixation Duration
ggplot(learned1, aes(bin, fixDuration)) +
  geom_boxplot(mapping = aes(as.factor(bin), fixDuration, color = condition)) +
  geom_smooth(method = "loess") +
  geom_smooth(mapping = aes(bin, y.hat), data = predict.fixDuration, method = "loess", color = 'red')

#Fixation Count
ggplot(learned, aes(bin, fixCount)) +
  geom_boxplot(mapping = aes(as.factor(bin), fixCount, color = condition)) +
  geom_smooth(method = "loess") +
  geom_smooth(mapping = aes(bin, y.hat), data = predict.fixCount, method = "loess", color = 'red')





#-----Model Fitting-----

#Accuracy
#testing for nonlinear effect
fit.accuracy1 <- lmer(meanAccuracy ~ bin + condition + (1 | subject), data = learned)
summary(fit.accuracy1)

fit.accuracy2 <- lmer(meanAccuracy ~ poly(bin, 2) + condition + (1 | subject), data = learned)
summary(fit.accuracy2)

fit.accuracy3 <- lmer(meanAccuracy ~ poly(bin, 3) + condition + (1 | subject), data = learned)
summary(fit.accuracy3)

fit.accuracy4 <- lmer(meanAccuracy ~ poly(bin, 4) + condition + (1 | subject), data = learned)
summary(fit.accuracy4)

fit.accuracy5 <- lmer(meanAccuracy ~ poly(bin, 5) + condition + (1 | subject), data = learned)
summary(fit.accuracy5)

anova(fit.accuracy1, fit.accuracy2, fit.accuracy3, fit.accuracy4, fit.accuracy5)
#4th order polynomial had the best fit



#Optimization
#testing if bin has a significant effect (that needs to be controlled)
fit.optimization1 <- lmer(optimization ~ condition + (1 | subject), data = learned)
summary(fit.optimization1)

fit.optimization2 <- lmer(optimization ~ bin + condition + (1 | subject), data = learned)
summary(fit.optimization2)

anova(fit.optimization1, fit.optimization2)
#bin is significant and should be included



#Fixation Duration
#testing for nonlinear effect
fit.fixDuration1 <- lmer(fixDuration ~ bin + condition + (1 | subject), data = learned1)
summary(fit.fixDuration1)

fit.fixDuration2 <- lmer(fixDuration ~ poly(bin, 2) + condition + (1 | subject), data = learned1)
summary(fit.fixDuration2)

fit.fixDuration3 <- lmer(fixDuration ~ poly(bin, 3) + condition + (1 | subject), data = learned1)
summary(fit.fixDuration3)

fit.fixDuration4 <- lmer(fixDuration ~ poly(bin, 4) + condition + (1 | subject), data = learned1)
summary(fit.fixDuration4)

fit.fixDuration5 <- lmer(fixDuration ~ poly(bin, 5) + condition + (1 | subject), data = learned1)
summary(fit.fixDuration5)

anova(fit.fixDuration1, fit.fixDuration2, fit.fixDuration3 , fit.fixDuration4, fit.fixDuration5)
#3rd order polynomial had the best fit



#Fixation Count
#testing for nonlinear effect
fit.fixCount1 <- lmer(fixCount ~ bin + condition + (1 | subject), data = learned)
summary(fit.fixCount1)

fit.fixCount2 <- lmer(fixCount ~ poly(bin, 2) + condition + (1 | subject), data = learned)
summary(fit.fixCount2)

fit.fixCount3 <- lmer(fixCount ~ poly(bin, 3) + condition + (1 | subject), data = learned)
summary(fit.fixCount3)

fit.fixCount4 <- lmer(fixCount ~ poly(bin, 4) + condition + (1 | subject), data = learned)
summary(fit.fixCount4)

anova(fit.fixCount1, fit.fixCount2, fit.fixCount3 , fit.fixCount4)
#3rd order polynomial had the best fit





#-----Assumptions-----

#Chi Square (independent observations and sample size over 10 for each cell in table)
#We have independent observations and a large sample size, so no assumptions are violated



#t test (normality, equal variance)

#Criterion Point:
#data are very skewed
hist(learned$cp[learned$condition == "3D"])
hist(learned$cp[learned$condition == "VR"])
#variance is similar in both groups
sd(learned$cp[learned$condition == "3D"])
sd(learned$cp[learned$condition == "VR"])

#Since normality is violated, we can test the median of the groups rather than mean

#Mann-Whitney U Test (independent groups, groups follow same distribution)
#groups are independent and data are skewed in the same direction in both groups, no assumptions violated



#Modelling


#Accuracy

#plot of residuals against fitted values
plot(fit.accuracy)
#suggests non-linearity (fixed)

#index plot of residuals
x <- c(1:length(resid(fit.accuracy)))
y <- c(resid(fit.accuracy))
plot(x, y, ylab = "Residuals", xlab = "Case Number")
abline (0,0)
#independent errors, no autocorrelation

#qq plot of residuals
qqnorm(resid(fit.accuracy))
qqline(resid(fit.accuracy))
#slight stray from normality

#variance inflation factor
vif(fit.accuracy)
#no multicollinearity

#shows any rows in data which residuals are greater than 2.5
res1 <- resid(fit.accuracy, type = "pearson")
learned[which(abs(res1) > 2.5),]
#no extreme outliers



#Optimization

#plot of residuals against fitted values
plot(fit.optimization)
#possible heteroscedasticity

#index plot of residuals
x <- c(1:length(resid(fit.optimization)))
y <- c(resid(fit.optimization))
plot(x, y, ylab = "Residuals", xlab = "Case Number")
abline (0,0)
#independent errors, no autocorrelation

#qq plot of residuals
qqnorm(resid(fit.optimization))
qqline(resid(fit.optimization))
#relatively normal

#variance inflation factor
vif(fit.optimization)
#no multicollinearity

#shows any rows in data which residuals are greater than 2.5
res1 <- resid(fit.optimization, type = "pearson")
learned[which(abs(res1) > 2.5),]
#no extreme outliers



#Fixation Duration

#plot of residuals against fitted values
plot(fit.fixDuration)
#slightly heteroscedastic, several extreme outliers (fixed), non-linear (fixed)

#index plot of residuals
x <- c(1:length(resid(fit.fixDuration)))
y <- c(resid(fit.fixDuration))
plot(x, y, ylab = "Residuals", xlab = "Case Number")
abline (0,0)
#independent errors, no autocorrelation

#qq plot of residuals
qqnorm(resid(fit.fixDuration))
qqline(resid(fit.fixDuration))
#slight stray from normality

#variance inflation factor
vif(fit.fixDuration)
#no multicollinearity

#shows any rows in data which residuals are greater than 2.5
res1 <- resid(fit.fixDuration, type = "pearson")
learned[which(abs(res1) > 2.5),]
#data are very scattered, lots of variance, but no other extreme outliers

#get the subject number of any fixations over 10000
learned$subject[learned$fixDuration > 10000]
#one trial from subject 20414 and all trials from subject 30513 (these trials are excluded)



#Fixation Count

#plot of residuals against fitted values
plot(fit.fixCount)
#slightly heteroscedastic, suggests non-linearity (fixed)

#index plot of residuals
x <- c(1:length(resid(fit.fixCount)))
y <- c(resid(fit.fixCount))
plot(x, y, ylab = "Residuals", xlab = "Case Number")
abline (0,0)
#independent errors, no autocorrelation

#qq plot of residuals
qqnorm(resid(fit.fixCount))
qqline(resid(fit.fixCount))
#slight stray from normality

#variance inflation factor
vif(fit.fixCount)
#no multicollinearity

#shows any rows in data which residuals are greater than 2.5
res1 <- resid(fit.fixCount, type = "pearson")
learned[which(abs(res1) > 2.5),]
#a lot of variance, subject 20420 trial 1 may be an outlier, but was not removed



