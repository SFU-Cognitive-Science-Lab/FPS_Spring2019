#Uncomment the install packages line if you don't have either of these installed
#install.packages("lme4")
library(lme4)
#install.packages("dplyr")
library(dplyr)
#install.packages("ggplot2")
library(ggplot2)

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



#-----ANOVA-----

#These ANOVAs are essentially basic lms to control for some of the stronger effects and see which variables VR differs from 3D
#Bonferroni correction must be considered for all output F statistics in each ANOVA test (.05/4 = .0125)
#Accuracy (transformed with square root due to non-linearity)
fit.aov <- aov(sqrt(meanAccuracy) ~ bin + learner + condition + as.factor(subject), data = dat)
summary(fit.aov)
#When controlling for bin, learner, and subject, condition approaches significance (F = 4.807, p = .0286)

#Optimization
fit.aov2 <- aov(optimization ~ bin + learner + condition + as.factor(subject), data = dat)
summary(fit.aov2)
#When controlling for bin, learner, and subject, condition has a significant effect (F = 108.218, p < 2e-16)

#Fixation Duration (multiple assumption violations, ANOVA might have high error rates)
fit.aov3 <- aov(fixDuration ~ bin + learner + condition + as.factor(subject), data = dat)
summary(fit.aov3)
#When controlling for bin, learner, and subject, condition does not have a significant effect (F = .499, p = .4799)

#Fixation Count (transformed with square root due to non-linearity)
fit.aov4 <- aov(sqrt(fixCount) ~ bin + learner + condition + as.factor(subject), data = dat)
summary(fit.aov4)
#When controlling for bin, learner, and subject, condition has a significant effect (F = 41.76, p = 1.6e-10)



#-----Modelling-----

#this is a boxplot with smooth line for the dataset (blue) and smooth line for best model (red)
ggplot(dat, aes(bin, meanAccuracy)) +
  geom_boxplot(mapping = aes(as.factor(bin), meanAccuracy)) +
  geom_smooth(method = "loess") +
  geom_smooth(mapping = aes(bin, y.hat), data = predict.mixed, method = "loess", color = 'red')



#best fit model for Accuracy (5th order polynomial and 1st order interactions) (added lag component)
fit.mixed9 <- lmer(meanAccuracy ~ poly(bin, 5) + learner + optimization + poly(fixCount, 5) + bin:optimization + 
  bin:fixCount + learner:optimization + learner:fixCount + optimization:fixCount + lag1 + (1 | subject), data = dat)
summary(fit.mixed9)
predict.mixed <- data.frame(y.hat = predict(fit.mixed9, dat), bin = dat$bin)



#-----Model Fitting-----

#base model
fit.mixed <- lmer(meanAccuracy ~ condition + bin + learner (1 | subject), data = dat)
summary(fit.mixed)

#add all theoretically relevant variables
fit.mixed2 <- lmer(meanAccuracy ~ condition + bin + learner + optimization + fixDuration + fixCount + (1 | subject), data = dat)
summary(fit.mixed2)

#adjust to statistically relevant variables
fit.mixed3 <- lmer(meanAccuracy ~ bin + learner + optimization + fixCount + (1 | subject), data = dat)
summary(fit.mixed3)

anova(fit.mixed2, fit.mixed3)
#fit.mixed doesn't have as many NAs, so it's a different size than the model 2 and 3 (anova throws an error)
#there is no significant increase in explained variance in model 2, so proceed with model 3



#test for nonlinear effects
#see assumption checks for plots and testing of which variables have nonlinear effects

#optimization was not compatible with the poly command, so I am currently leaving it out for nonlinear components
fit.mixed4 <- lmer(meanAccuracy ~ poly(bin, 2) + learner + optimization + poly(fixCount, 2) + (1 | subject), data = dat)
summary(fit.mixed4)

fit.mixed5 <- lmer(meanAccuracy ~ poly(bin, 3) + learner + optimization + poly(fixCount, 3) + (1 | subject), data = dat)
summary(fit.mixed5)

fit.mixed6 <- lmer(meanAccuracy ~ poly(bin, 4) + learner + optimization + poly(fixCount, 4) + (1 | subject), data = dat)
summary(fit.mixed6)

fit.mixed7 <- lmer(meanAccuracy ~ poly(bin, 5) + learner + optimization + poly(fixCount, 5) + (1 | subject), data = dat)
summary(fit.mixed7)

fit.mixed8 <- lmer(meanAccuracy ~ poly(bin, 6) + learner + optimization + poly(fixCount, 6) + (1 | subject), data = dat)
summary(fit.mixed8)

anova(fit.mixed3, fit.mixed4, fit.mixed5, fit.mixed6, fit.mixed7, fit.mixed8)
#with bonferroni correction (.05/5 = .01), fit.mixed7 is the best model (5th order polynomial)


#test for interaction effects
fit.mixed9 <- lmer(meanAccuracy ~ poly(bin, 5) + learner + optimization + poly(fixCount, 5) + bin:optimization + 
  bin:fixCount + learner:optimization + learner:fixCount + optimization:fixCount + (1 | subject), data = dat)
summary(fit.mixed9)

fit.mixed10 <- lmer(meanAccuracy ~ poly(bin, 5) + learner + optimization + poly(fixCount, 5) + bin:optimization + 
  bin:fixCount + learner:optimization + learner:fixCount + optimization:fixCount + bin:optimization:fixCount + 
  learner:optimization:fixCount + (1 | subject), data = dat)
summary(fit.mixed10)

anova(fit.mixed7, fit.mixed9, fit.mixed10)
#fit.mixed9 is the best overall



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



#ANOVA (errors are normally distributed, equal variance), robust to non-normality

#Accuracy
plot(fit.aov)
#suggests non-normality, slight heteroscedasticity

#Optimization
plot(fit.aov2)
#suggests non-normality

#Fixation Duration
plot(fit.aov3)
#non-normality, heteroscedastic, and a high outlier

#Fixation Count
plot(fit.aov4)
#non-normality





#Modelling

#I still need to complete all the assumption checking for the models



#looking for nonlinear effects
#bin
ggplot(data.frame(x1=dat$bin[is.na(dat$optimization) == F],pearson=residuals(fit.mixed3,type="pearson")),
       aes(x=x1,y=pearson)) +
  geom_point() +
  geom_smooth() +
  theme_bw()
#definitely nonlinear, maybe 2nd order

#optimization
ggplot(data.frame(x1=dat$optimization[is.na(dat$optimization) == F],pearson=residuals(fit.mixed2,type="pearson")),
       aes(x=x1,y=pearson)) +
  geom_point() +
  geom_smooth() +
  theme_bw()
#definitely nonlinear, maybe 3rd order

#fixation count
ggplot(data.frame(x1=dat$fixCount[is.na(dat$optimization) == F],pearson=residuals(fit.mixed2,type="pearson")),
       aes(x=x1,y=pearson)) +
  geom_point() +
  geom_smooth() +
  theme_bw()
#maybe nonlinear